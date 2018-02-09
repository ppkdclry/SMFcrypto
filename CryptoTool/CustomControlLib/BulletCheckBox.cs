using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlLib
{
    public class BulletCheckBox : CheckBox
    {
        static BulletCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BulletCheckBox), new FrameworkPropertyMetadata(typeof(BulletCheckBox)));
        }

        public BulletCheckBox()
        {
        }

        //protected override void OnChecked(RoutedEventArgs e)  
        //{  
        //    base.OnChecked(e);  
        //}  

        public static readonly DependencyProperty UncheckedTextProperty = DependencyProperty.Register(nameof(UncheckedText), typeof(string), typeof(BulletCheckBox),
                                                                                                    new PropertyMetadata(""));
        /// <summary>  
        /// 默认文本（未选中）  
        /// </summary>  
        public string UncheckedText
        {
            get { return (string)GetValue(UncheckedTextProperty); }
            set { SetValue(UncheckedTextProperty, value); }
        }

        public static readonly DependencyProperty CheckedTextProperty = DependencyProperty.Register(nameof(CheckedText), typeof(string), typeof(BulletCheckBox),
                                                                                                new PropertyMetadata(""));
        /// <summary>  
        /// 选中状态文本  
        /// </summary>  
        public string CheckedText
        {
            get { return (string)GetValue(CheckedTextProperty); }
            set { SetValue(CheckedTextProperty, value); }
        }

        public static readonly DependencyProperty CheckedForegroundProperty =
            DependencyProperty.Register(nameof(CheckedForeground), typeof(Brush), typeof(BulletCheckBox), new PropertyMetadata(Brushes.WhiteSmoke));
        /// <summary>  
        /// 选中状态前景样式  
        /// </summary>  
        public Brush CheckedForeground
        {
            get { return (Brush)GetValue(CheckedForegroundProperty); }
            set { SetValue(CheckedForegroundProperty, value); }
        }

        public static readonly DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register(nameof(CheckedBackground), typeof(Brush), typeof(BulletCheckBox),
                                                                new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 120, 215))));
        /// <summary>  
        /// 选中状态背景色  
        /// </summary>  
        public Brush CheckedBackground
        {
            get { return (Brush)GetValue(CheckedBackgroundProperty); }
            set { SetValue(CheckedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty IsTextOutsideProperty = DependencyProperty.Register(nameof(IsTextOutside), typeof(bool), typeof(BulletCheckBox),
                                                                new PropertyMetadata(false));
        /// <summary>  
        /// 文本显示在外面  
        /// </summary>  
        public bool IsTextOutside
        {
            get { return (bool)GetValue(IsTextOutsideProperty); }
            set { SetValue(IsTextOutsideProperty, value); }
        }

        public static readonly DependencyProperty IsViewBoxEnableProperty = DependencyProperty.Register(nameof(IsViewBoxEnable), typeof(bool), typeof(BulletCheckBox),
                                                                new PropertyMetadata(false));
        /// <summary>  
        /// 是否是能ViewBox缩放功能  
        /// </summary>  
        public bool IsViewBoxEnable
        {
            get { return (bool)GetValue(IsViewBoxEnableProperty); }
            set { SetValue(IsViewBoxEnableProperty, value); }
        }
    }
}
