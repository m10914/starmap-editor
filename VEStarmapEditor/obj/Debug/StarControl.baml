   M S B A M L   `   `   `  ����  VEStarmapEditor!    VEStarmapEditor.StarControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -   5          >�Name$  UserControl��5      .��5	      �� -   5         ��Name$ 
LayoutRoot��5	   
   $��	-8,-8,0,0q�6   '  !System.Windows.VisualStateManager   VisualStateGroups 5
      %  System.Windows.VisualStateGroup  5      -   5        VisualStateGroup��   States. 5         System.Windows.VisualState  -   5        Default��   
Storyboard. 5      �� .��5      T�  �� TargetProperty$ (UIElement.Visibility)�6&    �� 
TargetName$ ellipse��6Y   ..�5   	   i�  R� KeyTime$ 0��6      System.Windows.Visibility   	Collapsed	 R� Value#	 Z 6,   5   	   5      5        5      -   5        Selected��. 5      �� .��5      T� $ (UIElement.Visibility)�6&   $ ellipse��6Y   ..�5   	   i� $ 0��6    
   Visible#	 Z
 6,   5   	   r� 5      $ (FrameworkElement.Margin)�6)   $ ellipse��6_   .	�5   	   <  6System.Windows.Media.Animation.EasingThicknessKeyFrame   p� KeyTime$ 0��6!    p� Value$ 2,2,0,0q�6-   5   	   U� 5      $U O(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.Y)�6&   $ ellipse��6�   .\�5   	   9  3System.Windows.Media.Animation.EasingDoubleKeyFrame   O� KeyTime$ 0��6    O� Value$ -1.915Q�6*   5   	   U� 5      $U O(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)�6&   $ ellipse��6�   .\�5   	     $ 0��6   $ -1.914Q�6*   5    	   5!      5"      5#      .+�5%      �� -   5         ��Name$ StarIcon��5%   
   $��16��6   $��16��6(   $��Img/Star.png��63   $��0q�6I   $
��Left=�6T   $	��Top=�6o   �� 5&      -   5         ��Name$ 	TechLevel��5&   
   $
��23
��6   $��5.333(�64   $��Disabled=�6E   $��Right=�6y   $	��Top=�6�   $��0q�6�   $��	0,0,-25,0q�6�   ���	#FFE9FF09 J� Opacity$
 0.78Q�6�   ��6�   Z� 6�   �� 5'      -   5        $ DangerLevel��5'   
   $	��23
��6   $��5.333(�61   $��Disabled=�6B   $
��Left=�6v   $	��Top=�6�   $��0q�6�   ���	#FFFF7E7E$
 0.78Q�6�    �� Left$ 19.373��6�   $��-8,0,0,0q�6  ��6�   Z� 6  A� 5(      -	   5         A�Name$ ellipse��5(      $
��Left=�6   $��14��68   ���	#FF849FFD$	��Top=�6W   $��14��6o    J� RenderTransformOrigin$ 0.5,0.5&�6z   $	��0.5��6�   $ 1.085��6�   
 �� Top$ 1.085��6�   $y�	Collapsed=�6�   z�5)      V� 5*      .~�5+      �� �� 5,      �� 5-      T� 5.      5/      51      �� 52      -
   5        $ StarName��52   
   $!��Alderaanasd asd asd asd asd��6   $��0q�6B   $��8(�6M   
�������$
 0.39Q�6�   $��Center=�6�   $��Bottom=�6�   $��Center=�6�   $��Disabled=�6�    J� SnapsToDevicePixels . $ 15.085��6;  $
 -142��6O  $��Center=�6b  $	��300��6�  $��0q�6�  $��Segoe UI Light)�6�  ��6Z   Z� 6�  �� 53      -   5        $ TechLevel_Copy��53   
   $��/
��6"   $��6.667(�63   $��Disabled=�6D   $��Right=�6x   $	��Top=�6�   $��0q�6�   ���	#FFFFFFFB$
 0.78Q�6�   $ 19.253��6�   $��	-7,-1,0,0q�6	  ��6�   Z� 6	  54      55      