using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControlLib
{
    public class ImageTextBox : TextBox
    {
        private ImageButton mClearTextButton = null;
        private ImageButton mExtendIconFontButton = null;
        private ImageButton mExtendImageButton = null;


        static ImageTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageTextBox), new FrameworkPropertyMetadata(typeof(ImageTextBox)));
        }


        public ImageTextBox()
        {

        }


        public override void OnApplyTemplate()
        {
            mClearTextButton = GetTemplateChild("PART_ClearTextButton") as ImageButton;
            if (null != mClearTextButton)
            {
                mClearTextButton.Click += OnClearTextClick;
            }


            mExtendIconFontButton = GetTemplateChild("PART_ExtendIconFontButton") as ImageButton;
            if (null != mExtendIconFontButton)
            {
                mExtendIconFontButton.Click += ExtendButtonClick;
            }


            mExtendImageButton = GetTemplateChild("PART_ExtendImageButton") as ImageButton;
            if (null != mExtendIconFontButton)
            {
                mExtendImageButton.Click += ExtendButtonClick;
            }


            base.OnApplyTemplate();
        }


        private void ExtendButtonClick(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs re = new RoutedEventArgs(ImageTextBox.ExtendClickEvent, this);
            RaiseEvent(re);
        }


        private void OnClearTextClick(object sender, RoutedEventArgs e)
        {
            if (sender is ImageButton)
            {
                Text = "";
            }
        }


        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(ImageTextBox),
                                                                                                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }


        public static readonly DependencyProperty ExtendNormalImageProperty = DependencyProperty.Register(nameof(ExtendNormalImage), typeof(ImageSource), typeof(ImageTextBox),
                                                                                        new FrameworkPropertyMetadata(null));
        public ImageSource ExtendNormalImage
        {
            get { return (ImageSource)GetValue(ExtendNormalImageProperty); }
            set { SetValue(ExtendNormalImageProperty, value); }
        }


        public static readonly DependencyProperty ExtendHoverImageProperty = DependencyProperty.Register(nameof(ExtendHoverImage), typeof(ImageSource), typeof(ImageTextBox),
                                                                                        new FrameworkPropertyMetadata(null));
        public ImageSource ExtendHoverImage
        {
            get { return (ImageSource)GetValue(ExtendHoverImageProperty); }
            set { SetValue(ExtendHoverImageProperty, value); }
        }


        public static readonly DependencyProperty ExtendPressedImageProperty = DependencyProperty.Register(nameof(ExtendPressedImage), typeof(ImageSource), typeof(ImageTextBox),
                                                                                        new FrameworkPropertyMetadata(null));
        public ImageSource ExtendPressedImage
        {
            get { return (ImageSource)GetValue(ExtendPressedImageProperty); }
            set { SetValue(ExtendPressedImageProperty, value); }
        }


        public static readonly DependencyProperty ExtendDisabledImageProperty = DependencyProperty.Register(nameof(ExtendDisabledImage), typeof(ImageSource), typeof(ImageTextBox),
                                                                                        new FrameworkPropertyMetadata(null));
        public ImageSource ExtendDisabledImage
        {
            get { return (ImageSource)GetValue(ExtendDisabledImageProperty); }
            set { SetValue(ExtendDisabledImageProperty, value); }
        }


        public static readonly DependencyProperty PlaceHolderProperty = DependencyProperty.Register(nameof(PlaceHolder), typeof(string),
                                                                    typeof(ImageTextBox), new PropertyMetadata(""));
        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }


        public static readonly DependencyProperty ExtendIconFontTextProperty = DependencyProperty.Register(nameof(ExtendIconFontText), typeof(string),
                                                                    typeof(ImageTextBox), new PropertyMetadata(""));
        public string ExtendIconFontText
        {
            get { return (string)GetValue(ExtendIconFontTextProperty); }
            set { SetValue(ExtendIconFontTextProperty, value); }
        }


        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius),
            typeof(ImageTextBox), new PropertyMetadata(new CornerRadius(0, 0, 0, 0)));
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }


        public static readonly DependencyProperty LeftImageWidthProperty = DependencyProperty.Register(nameof(LeftImageWidth), typeof(double),
                                                                                        typeof(ImageTextBox), new PropertyMetadata(0.0));
        public double LeftImageWidth
        {
            get { return (double)GetValue(LeftImageWidthProperty); }
            set { SetValue(LeftImageWidthProperty, value); }
        }


        private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ImageTextBox itb = sender as ImageTextBox;
            ImageSource image = e.NewValue as ImageSource;


            itb.SetValue(LeftImageWidthProperty, image.Width);
        }


        public static readonly RoutedEvent ExtendClickEvent = EventManager.RegisterRoutedEvent(nameof(ExtendClick), RoutingStrategy.Bubble,
                                                                            typeof(RoutedEventHandler), typeof(ImageTextBox));


        public event RoutedEventHandler ExtendClick
        {
            add { AddHandler(ExtendClickEvent, value); }
            remove { RemoveHandler(ExtendClickEvent, value); }
        }


        public static DependencyProperty ExtendButtonCommandProperty = DependencyProperty.Register(nameof(ExtendButtonCommand), typeof(ICommand), typeof(ImageTextBox));
        public ICommand ExtendButtonCommand
        {
            get { return (ICommand)GetValue(ExtendButtonCommandProperty); }
            set { SetValue(ExtendButtonCommandProperty, value); }
        }


        public static readonly DependencyProperty ExtendButtonCommandParameterProperty =
                                        DependencyProperty.Register(nameof(ExtendButtonCommandParameter), typeof(object), typeof(ImageTextBox), new UIPropertyMetadata(null));
        public object ExtendButtonCommandParameter
        {
            get { return (object)GetValue(ExtendButtonCommandParameterProperty); }
            set { SetValue(ExtendButtonCommandParameterProperty, value); }
        }


        public static readonly DependencyProperty ExtendButtonCommandTargetProperty =
                                    DependencyProperty.Register(nameof(ExtendButtonCommandTarget), typeof(IInputElement), typeof(ImageTextBox), new UIPropertyMetadata(null));
        public IInputElement ExtendButtonCommandTarget
        {
            get { return (IInputElement)GetValue(ExtendButtonCommandTargetProperty); }
            set { SetValue(ExtendButtonCommandTargetProperty, value); }
        }
    }
}
