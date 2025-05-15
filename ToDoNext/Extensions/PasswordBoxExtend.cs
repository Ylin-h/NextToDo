using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static MaterialDesignThemes.Wpf.Theme;

namespace ToDoNext.Extensions
{
    public class PasswordBoxExtend
    {


        public static string  GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxExtend), new PropertyMetadata("", OnPasswordChanged));

        /// <summary>
        /// 密码变化时触发
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.PasswordBox passwordBox = d as System.Windows.Controls.PasswordBox;
            if (passwordBox != null)
            { 
                passwordBox.Password = (string)e.NewValue;
                SetSelection(passwordBox, passwordBox.Password.Length, 0);

            }
        }
        /// <summary>
        /// 设置光标位置
        /// </summary>
        /// <param name="passwordBox"></param>
        /// <param name="start">光标开始位置</param>
        /// <param name="length">选中长度</param>
        private static void SetSelection(System.Windows.Controls.PasswordBox passwordBox, int start, int length)
        {
            passwordBox.GetType()
                      .GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic)
                      .Invoke(passwordBox, new object[] { start, length
          });
        }


    }
    /// <summary>
    /// 自定义附加属性行为，password变化时触发
    /// </summary>
    public class PasswordBoxBehavior:Behavior<System.Windows.Controls.PasswordBox>
    {
        /// <summary>
        /// 附加行为时触发
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PasswordChanged += PasswordBoxBehavior_PasswordChanged;
        }
        /// <summary>
        /// 移除行为时触发
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PasswordChanged -= PasswordBoxBehavior_PasswordChanged;
        }
        /// <summary>
        /// password密码变化时触发,PasswordBox的Password属性也跟着变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBoxBehavior_PasswordChanged(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PasswordBox passwordBox = sender as System.Windows.Controls.PasswordBox;
            if (passwordBox != null)
            {
                PasswordBoxExtend.SetPassword(passwordBox, passwordBox.Password);
            }
        } 
    }
}
