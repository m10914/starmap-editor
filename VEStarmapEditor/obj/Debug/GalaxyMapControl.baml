   M S B A M L   `   `   `  ����  VEStarmapEditor&     VEStarmapEditor.GalaxyMapControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -   5          >�Name$  UserControl��5      .��5      � -   5         �Name$ 
LayoutRoot��5      
���   �.+�5      � -   5        $ Controls��5   	   $
��Left=�6   $	��Top=�66   $	��281��6N   $��17,0,0,0q�6Z    � ColumnDefinitions 5      �� 5      �� 5       � RowDefinitions 5      �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5       $
��Auto �6   �� 5!      $
��Auto �6   �� 5"      $
��Auto �6   �� 5#      $
��Auto �6   .+�5%      �� $��Editor controls��6   $
��Left=�6%   $	��Top=�6@   
�������$��5,5,5,16q�6�   ��6X   Z� 6�   �� 5&      -   5         ��Name$ GenerateButton��5&      $��RandomGenerate��6$   $
��Left=�6=   $	��Top=�6X    � Cursor$
 Hand��6p    K� ZIndex$ 56��6~   $��1��6�    � ToolTip$. (Random generation by specified algorythm��6�   $	��114��6�   $��0,0,0,9q�6�   �� 5'      -   5        $ 	CenterMap��5'      $��Center��6   $
��Left=�60   $	��Top=�6K   $
 Hand��6c   $ 56��6q   $��1��6�   $ Centers canvas��6�   $��68��6�   $��1��6�   �� 5(      -   5        $ ScriptButton��5(      $��ScriptGenerate��6"   $
��Left=�6;   $	��Top=�6V   $
 Hand��6n   $ 56��6|   $��2��6�   $&  Generate systems based on script��6�   $��98��6�   �� 5)      -   5        $ ClearButton��5)      $��Clear��6!   $
��Left=�61   $	��Top=�6L   $
 Hand��6d   $ 56��6r   $5 /Removes all currently created stars, links, etc��6�   $��98��6�   $��3��6�   $��0,6,0,0q�6�   5*      �� 5+      -   5         ��Name$ 
RootCanvas��5+      $ -6��6   	 J� Clip	 5,      �� 5-      -	   5        ClippingRect��
 �� Rect$
 0,0,1000,1000�5-   :   .+�5/      �� -
   5        $ 	LowCanvas��5/      $
��1000��6   $
��1000��6,   $
��Left=�6:   $	��Top=�6U   z�50      V� 51      .~�52      �� -   5        CanvasContentScaleTransform�� �� ScaleX$ 1Q�52   N    �� ScaleY$ 1Q�6Y   T� 53      -   5        #CanvasContentTranslateTransform��53   -   54      ��56      �� 57       �� ImageSource$ Img/background.jpg��6    j� TileMode$
 Tile=�63    �� RelativeTransform 58      V� 59   	   .~�5:   
   ��  �� CenterY$	 0.5Q�6    �� CenterX$	 0.5Q�6'   $	 0.2Q�65   $	 0.2Q�6B   �� 5;   
    �� CenterY$	 0.5Q�6    �� CenterX$	 0.5Q�6&   �� 5<   
    �� CenterY$	 0.5Q�6    �� CenterX$	 0.5Q�6(   T� 5=   
   5>   
   5@      5B      5C      �� 5D      -   5         ��Name$ CurrentStatus��5D   
   $��CurrentStatus��6!   $��Bottom=�69   ���	#FF555555$��26��6k   ���	#FFFFFBFB���	#FFEEE2E26�   �� 5E      -   5         ��Name$ ConsoleHistory��5E      $��Bottom=�6'   $��2��6B   ���	#FFBFBDBD$��0,0,0,27q�6m   .+�5F      �� $��Label��6      ConsoleLabelStyle#���   6   �� 5G      $��Label��6   #���   6   5H      5I      5J      