   M S B A M L   `   `   `  ����>  :RAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:RAPRAP  5   	   %clr-namespace:RAP.Model	RAP.Model  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   #clr-namespace:RAP.ViewRAP.View  5	   	       RAP.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:RAP     5   	   )Modelclr-namespace:RAP.Model     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   'Viewclr-namespace:RAP.View     5	   	   -   5        $��MainWindowObject��5   	     � Title$  
MainWindow��5   	   $
��1035��6   $	��606��6)   $
��1035��66   $	��606��6F   .��5      �  � RowDefinitions 5   
   �� 5      $��24 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $	��204 �6   �� 5      �� 5      $	��204 �6   .+�5   
   �� 
���   �$��1q�6%   $��3��69   $
��Auto��6M   ._�5      �� -   5        $��Generate Reports��5      $��9.2,1.2,907.2,0.8q�60   $
��Auto��6d   $	��100��6r   $
��Left=�6~   5      !   RAP.View.ResearcherListView  5   
   -   5          Name$ ResearcherListView��5   "   $
��Auto��6>   $
��Auto��6K   $��1��6Y   $��Stretch=�6f   $   RAP.View.ResearcherDetailsView  5!   
   -   5          Name$ ResearcherDetailView��5!   %   $��Stretch=�6C   $��Stretch=�6a   $��1��6}   $��1��6�   �� 5#   
   
�������$��1q�6$   $��2��68   $��1��6H   $
��Auto��6U   $
��Auto��6b   $��Stretch=�6p   ._�5$      �� .@�5&      �� $��Supervisions��6   $��0q�60   $��81��6;   .��5'      #   RAP.View.SupervisionsListView  -   5          Name$ SupervisionsListView��5'   0   5(      �� 5*      $��Cumulative Count��6   $��0q�64   $	��117��6?   .��5+      "   RAP.View.CumulativeCountView  -   5          Name$ CumulativeCountView��5+   /   5,      �� 5.      $��Publication Details��6   $��0q�67   $	��198��6B   .��5/      %   RAP.View.PublicationDetailsView  -   5          Name$ PublicationsDetailsView��5/   2   50      51      52      53      54      