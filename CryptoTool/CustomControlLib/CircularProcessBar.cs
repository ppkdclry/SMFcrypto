using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomControlLib
{
    public class CircularProcessBar : ProgressBar
    {

        static CircularProcessBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularProcessBar), new FrameworkPropertyMetadata(typeof(CircularProcessBar)));
        }


        public CircularProcessBar()
        {

        }
    }
}
