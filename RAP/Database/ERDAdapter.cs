using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RAP.Database
{
    abstract class ERDAdapter
    {
        //Db login credentials
        private const string db = "kit206";
        private const string un = "kit206";
        private const string pw = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        //Db connection
        private static MySqlConnection db_conn = null;

        /// <summary>
        /// Initialises the database connection if no connection has already been made
        /// </summary>
        private static void GetConnection()
        {
            if(db_conn == null)
            {
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, un, pw);
                db_conn = new MySqlConnection(connectionString);
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// <para>Gets the basic researcher details from the database</para>
        /// <para>Used to populate the Researcher List View</para>
        /// </summary>
        /// <returns>
        /// A list of researhcers containing title, given name and family name of all researhcers
        /// </returns>
        public static List<Model.Researcher> FetchBasicResearcherDetails()
        {
            List<Model.Researcher> researchers = new List<Model.Researcher>();
            MySqlDataReader rdr = null; //Data Reader for MySQL queries

            GetConnection();

            try
            {
                //Open database connection
                db_conn.Open();

                //Create command, including database connection
                MySqlCommand cmd = new MySqlCommand("SELECT id, type, title, given_name, family_name, supervisor_id, email FROM researcher ORDER BY family_name, given_name", db_conn);

                //Get query results
                rdr = cmd.ExecuteReader();

                //Add each row to list
                while(rdr.Read())
                {
                    switch (rdr.GetString(1))
                    {
                        case "Student":
                            researchers.Add(new Model.Student
                            {
                                ID = rdr.GetInt32(0),
                                Type = rdr.GetString(1),
                                Title = rdr.GetString(2),
                                GivenName = rdr.GetString(3),
                                FamilyName = rdr.GetString(4),
                                Supervisor = rdr.GetInt32(5),
                                Email = rdr.GetString(6)
                            });
                            break;
                        case "Staff":
                            researchers.Add(new Model.Staff
                            {
                                ID = rdr.GetInt32(0),
                                Type = rdr.GetString(1),
                                Title = rdr.GetString(2),
                                GivenName = rdr.GetString(3),
                                FamilyName = rdr.GetString(4),
                                Email = rdr.GetString(6)
                            });
                            break;
                    }
                }
            }
            //Catch any database errors
            catch(MySqlException e)
            {
                Console.WriteLine("Error: Cannot connect to database " + e);
            }
            //Close reader and db connection
            finally
            {
                if(rdr != null)
                {
                    rdr.Close();
                }

                if(db_conn != null)
                {
                    db_conn.Close();
                }
            }

            GetConnection();
            //Get researcher positions
            foreach(Model.Researcher r in researchers)
            {
                if(r is Model.Staff)
                {
                    r.Positions = FetchPositions(r.ID);
                }
                else
                {
                    r.Positions = new List<Model.Position>();
                    r.Positions.Add(new Model.Position { Level = Model.EmploymentLevel.Student, Start = default(DateTime), End = default(DateTime) });
                }
            }
            
            return researchers;
        }

        /// <summary>
        /// <para>Gets the details of a given researcher from the database</para>
        /// <para>Used to populate the Researcher Details View</para>
        /// </summary>
        /// <param name="id">The database ID number of the given researcher</param>
        /// <returns>A complete researcher object</returns>
        public static Model.Researcher FetchFullResearcherDetails(int id, List<Model.Researcher> researchers)
        {
            Model.Researcher researcher = null; //Researcher data object being returned
            MySqlDataReader rdr = null; //MySQL data reader
            

            GetConnection();

            try
            {
                //Open database connection
                db_conn.Open();

                //Create command, including database connection
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM researcher WHERE id=?id", db_conn);
                cmd.Parameters.AddWithValue("id", id);

                //Get query results
                rdr = cmd.ExecuteReader();

                //Add resulting object to Researcher object
                while(rdr.Read())
                {
                    switch(rdr.GetString(1))
                    {
                        case "Student":
                            Model.Student student = new Model.Student
                            {
                                ID = rdr.GetInt32(0),
                                Type = rdr.GetString(1),
                                GivenName = rdr.GetString(2),
                                FamilyName = rdr.GetString(3),
                                Title = rdr.GetString(4),
                                School = rdr.GetString(5),
                                Campus = rdr.GetString(6),
                                Email = rdr.GetString(7),
                                Photo = new Uri(rdr.GetString(8), UriKind.Absolute),
                                Degree = rdr.GetString(9),
                                Supervisor = rdr.GetInt32(10)
                            };
                            student.Publications = new List<Model.Publication>();
                            student.Positions = new List<Model.Position>();
                            student.Positions.Add(new Model.Position { Level = Model.EmploymentLevel.Student, Start = rdr.GetDateTime(12), End = default(DateTime) });

                            //Get supervisor
                            foreach(Model.Researcher i in researchers)
                            {
                                if(student.Supervisor == i.ID)
                                {
                                    student.SupervisorName = i.FamilyName + ", " + i.GivenName + " (" + i.Title + ")";
                                }
                            }
                            researcher = student;
                            break;
                        case "Staff":
                            //Get any supervisions the staff member
                            List<Model.Student> supervisions = new List<Model.Student>();
                            foreach (Model.Researcher i in researchers)
                            {
                                if (i.Type == "Student")
                                {
                                    Model.Student studentCast = (Model.Student)i;
                                    if (rdr.GetInt32(0) == studentCast.Supervisor)
                                    {
                                        supervisions.Add(studentCast);
                                    }
                                }
                                
                            }
                            researcher = new Model.Staff
                            {
                                ID = rdr.GetInt32(0),
                                Type = rdr.GetString(1),
                                GivenName = rdr.GetString(2),
                                FamilyName = rdr.GetString(3),
                                Title = rdr.GetString(4),
                                School = rdr.GetString(5),
                                Campus = rdr.GetString(6),
                                Email = rdr.GetString(7),
                                Photo = new Uri(rdr.GetString(8), UriKind.Absolute),
                                Supervisions = new List<Model.Student>(supervisions),
                                
                               
                            };
                            researcher.Publications = new List<Model.Publication>();
                            break;
                    }
                }
            }
            //Catch any database errors
            catch(MySqlException e)
            {
                Console.WriteLine("Error: Cannot connect to database " + e);
            }
            //Close database connection and data reader
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (db_conn != null)
                {
                    db_conn.Close();
                }
            }
            
            //Get any previous positions that staff members have held
            if(researcher.Type != "Student")
            {
                researcher.Positions = FetchPositions(researcher.ID);
            }
            return researcher;
        }

        /// <summary>
        /// <para>Returns all the positions held by a given researcher</para>
        /// </summary>
        /// <param name="id">The database ID number of the given researcher</param>
        /// <returns>A list of positions pertaining to the given researcher</returns>
        private static List<Model.Position> FetchPositions(int id)
        {
            List<Model.Position> positions = new List<Model.Position>();
            MySqlDataReader rdr = null;

            GetConnection();

            try
            {
                //Open Database connection
                db_conn.Open();

                //Create command, including database connection
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM position WHERE id=?id", db_conn);
                cmd.Parameters.AddWithValue("id", id);
                
                //Get query results
                rdr = cmd.ExecuteReader();

                //Add results to C# object
                while(rdr.Read())
                {
                    //check that the end value in database is not null, if it is then set end to a min value
                    DateTime end;
                    if (DBNull.Value.Equals(rdr["end"]))
                    {
                        end = default(DateTime);
                    } 
                    else
                    {
                        end = Convert.ToDateTime(rdr["end"]);
                    }
                    //add data to position
                    positions.Add(new Model.Position {
                        Level = ParseEnum<Model.EmploymentLevel>(rdr.GetString(1)),
                        Start = Convert.ToDateTime(rdr["start"]),
                        End = end
                    });
                }
            }
            //Catch any database errors
            catch(MySqlException e)
            {
                Console.WriteLine("Error: Cannot connect to database " + e);
            }
            //Close data reader
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if(db_conn != null)
                {
                    db_conn.Close();
                }
            }

            return positions;
        }

        /// <summary>
        /// returns basic publication details for a researcher
        /// </summary>
        /// <param name="researcher"></param>
        /// <returns>A list of publications</returns>
        public static List<Model.Publication> fetchBasicPublicationDetails(Model.Researcher researcher)
        {
            int id = researcher.ID;
            List<Model.Publication> publications = new List<Model.Publication>();
            List<string> dois = new List<string>();

            MySqlDataReader rdr = null;

            GetConnection();


            try
            {
                //Open Database connection
                db_conn.Open();

                //Create command, including database connection 
                MySqlCommand cmd = new MySqlCommand("SELECT doi FROM researcher_publication WHERE researcher_id=?id", db_conn); //get the ids of the publications for the researcher
                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();

                while(rdr.Read())
                {
                    dois.Add(rdr.GetString(0));
                }
              
            }
            //Catch any database errors
            catch (MySqlException e)
            {
                Console.WriteLine("Error: Cannot connect to database " + e);
            }
            //Close data reader
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
       
                }
                if (db_conn != null)
                {
                    db_conn.Close();
                }
            }


            //for each id, fetch the basic publication details
            foreach (string doi in dois)
            {

                try
                {
                    //Open Database connection
                    db_conn.Open();

                    //Create command, including database connection
                    MySqlCommand cmd = new MySqlCommand("SELECT title, year FROM publication WHERE doi = ?doi", db_conn);
                    cmd.Parameters.AddWithValue("doi", doi);
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        publications.Add(new Model.Publication
                        {
                            DOI = doi,
                            Title = rdr.GetString(0),
                            Year = new DateTime(rdr.GetInt32(1), 1, 1)//rdr.GetDateTime(1)
                    });
                    }

                    rdr.Close();
                }
                //Catch any database errors
                catch (MySqlException e)
                {
                    Console.WriteLine("Error: Cannot connect to database " + e);
                }
                //Close data reader
                finally
                {
                    if (rdr != null)
                    {
                        rdr.Close();

                    }
                    if (db_conn != null)
                    {
                        db_conn.Close();
                    }
                }
            }

            //sort publications list
            publications = new List<Model.Publication>(from Model.Publication p in publications
                                                       orderby p.Year descending, p.Title ascending
                                                       select p);

            return publications;
        }

        public static Model.Publication fetchFullPublicationDetails(Model.Publication publication)
        {
            MySqlDataReader rdr = null;

            //Get Connection
            GetConnection();

            try
            {
                //open connection
                db_conn.Open();

                //load publication details
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM publication WHERE doi = ?doi", db_conn);
                cmd.Parameters.AddWithValue("doi", publication.DOI);
                rdr = cmd.ExecuteReader();

                rdr.Read();

                publication.Authors = rdr.GetString(2);
                publication.Type = ParseEnum<Model.OutputType>(rdr.GetString(4));
                publication.CiteAs = rdr.GetString(5);
                publication.Available = Convert.ToDateTime(rdr["available"]);
            }
            catch(MySqlException e)
            {
                Console.WriteLine("Error cannot connect to database " + e);
            }
            //Close data reader
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();

                }
                if (db_conn != null)
                {
                    db_conn.Close();
                }
            }

            return publication;
        }
    }
}