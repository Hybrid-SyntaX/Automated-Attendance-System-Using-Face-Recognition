using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AAS.GUI.Controls
{
    /// <summary>
    /// Interaction logic for AASMessageBox.xaml
    /// </summary>
    public partial class AASMessageBox : UserControl
    {





        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(AASMessageBox), new PropertyMetadata(string.Empty));


        public string YesText
        {
            get { return (string)GetValue(YesTextProperty); }
            set { SetValue(YesTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YesText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YesTextProperty =
            DependencyProperty.Register("YesText", typeof(string), typeof(AASMessageBox), new PropertyMetadata(string.Empty));



        public string NoText
        {
            get { return (string)GetValue(NoTextProperty); }
            set { SetValue(NoTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NoText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoTextProperty =
            DependencyProperty.Register("NoText", typeof(string), typeof(AASMessageBox), new PropertyMetadata(string.Empty));



        public AASMessageBox()
        {
            InitializeComponent();
            //int a=0x7F00FF17;
         //   Background = new SolidColorBrush(Color.FromArgb(0x7F, 0xFF, 0xFF, 0x17));
            this.Background = new SolidColorBrush(Color.FromArgb(0x7F, 0xFF, 0xFF, 0x17));

  
        }
    }
}
