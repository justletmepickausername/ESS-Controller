   M S B A M L   `   `   `  ���� I  EESS Controller, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ESS_Controller.WindowsESS_Controller.Windows  U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf &     ESS_Controller.Windows.ESSWindow   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6local$clr-namespace:ESS_Controller.Windows     T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3f6mah,http://metro.mahapps.com/winfx/xaml/controls  >lvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf  -   $
��Main��  � WindowStartupLocation$  CenterScreen=�(  "MahApps.Metro.Controls.MetroWindow   TitleCharacterCasing$ Normal=� � 
ResizeMode$ CanMinimize=� � Title$ ESS Controller��$	��750��$
��1400�� � 	Resources  �� 
TargetType �� Property �� Value��%0   '��       �� " ��.���  � |�$ False��.��� .+��� -   $��txtblockTitle��$��Center=�$	��Top=�$��0,25,0,0q�$��ESS Controller��$��	Underline{�$��30(�$
��Wrap=��� -   $��txtblockPorts��$
��Left=�$��	56,0,0,40q�$��Port:��$
��Wrap=�$��Bottom=�$��17(��� -   $��comboBoxPortsList��$
��Left=�$��
100,0,0,38q�$��Bottom=�$	��220���� -   $��txtboxMaxTemp��$��60��$
��Left=�$��322,545,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��120���� -   $��txtblockMaxTemp��$��230,545,0,0q�$��82��$��	Max Temp:��$
��Wrap=�$	��Top=�$��17(�$
��Left=��� -   $��btnStartStopESS��+  %MahApps.Metro.Controls.ControlsHelper	   ContentCharacterCasing$	 Normal=�$��Start��$��Center=�$��	0,620,0,0q�$	��Top=�$��20(�$	��100��$��40���� -   
 ��Name$
 txtboxMinTemp��$	��-30��$
��Left=�$��574,545,0,0q�$
 Wrap=�$	��Top=�$	��120���� -	   $��txtblockMinTemp��$
��Left=�$��82��$��482,545,0,0q�$��	Min Temp:��$
��Wrap=�$	��Top=�$��17(��� -
   $
 txtboxCycles��$��9��$
��Left=�$��826,545,0,0q�$
 Wrap=�$	��Top=�$	��120���� -   $��txtblockCycles��$
��Left=�$��55��$��734,545,0,0q�$��Cycles:��$
��Wrap=�$	��Top=�$��17(��� -   $
 txtboxStayTime��$��30��$
��Left=�$��1078,545,0,0q�$
 Wrap=�$	��Top=�$	��120���� -   $��txtblockStayTime��$
��Left=�$��82��$��986,545,0,0q�$��
Stay Time:��$
��Wrap=�$	��Top=�$��17(��� -   $��listviewLog��$��826,595,10,10q�$	��0.5q�
���   �$��11(��� $��Right=�$��	0,20,20,0q�$	��Top=�$	��350��$��40��$��/Windows/Opsys Logo.png��#  LiveCharts.Wpf.CartesianChart  -   $��	ovenGraph��$
��1190��$	��375��$��	0,120,0,0q�$��Center=�$	��Top=�&   LiveCharts.Wpf.Charts.Base.Chart   Series   LiveCharts.Wpf.LineSeries  -   $��mySeries��  LiveCharts.Wpf.Series   Fill
 ����    Values;  5LiveCharts.Wpf.Converters.NumericChartValuesConverter$   �� -   $��txtBlockPath��$
��Left=�$��100,627,0,0q�$��R:\Lidar\Just\��$
 Wrap=�$	��Top=�$	��220��