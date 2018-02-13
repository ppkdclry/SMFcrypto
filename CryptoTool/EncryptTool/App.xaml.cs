using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Reflection;

namespace EncryptTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SetEncryptRegistryKey();
            SetDecryptRegistryKey();
            MainWindow mWind;
            if(e.Args.Length == 0)
            {
                mWind = new MainWindow();
            }else{
                mWind = new MainWindow(e.Args);
            }
            mWind.Show();
        }

        /// <summary>
        /// 检查注册表项是否存在，如果存在则不做什么，如果不存在则设置
        /// 出现权限不足或者其他异常情况默认跳过
        /// </summary>
        private void SetEncryptRegistryKey()
        {
            RegistryKey reg = null;
            RegistryKey newReg = null;
            try
            {
                reg = Registry.LocalMachine;
                reg = reg.OpenSubKey(@"Software\Classes\*\shell\EncryptTool");
                //如果注册表不存在该项，则创建
                if (null == reg)
                {
                    newReg = Registry.LocalMachine;
                    newReg = newReg.CreateSubKey(@"Software\Classes\*\shell\EncryptEntry");
                    newReg.SetValue("", "将文件加密");
                    if (newReg != null) { newReg.Close(); newReg = null; }
                }

                //再次检查是否存在EncryptTool项
                reg = Registry.LocalMachine;
                reg = reg.OpenSubKey(@"Software\Classes\*\shell\EncryptEntry", true);
                if(reg != null)
                {
                    //运行至此右键菜单应该已经创建，下面设置图标
                    object temp = reg.GetValue("Icon");
                    if (temp == null)
                    {
                        //设置图标
                        reg.SetValue("Icon", Assembly.GetExecutingAssembly().Location);
                        reg.Close(); reg = null;
                    }

                    //设置右键菜单命令
                    reg = Registry.LocalMachine;
                    reg = reg.OpenSubKey(@"Software\Classes\*\shell\EncryptEntry\command");
                    //如果存在该注册表项则跳过
                    if (null == reg)
                    {
                        newReg = Registry.LocalMachine;
                        newReg = newReg.CreateSubKey(@"Software\Classes\*\shell\EncryptEntry\command");
                        newReg.SetValue("", Assembly.GetExecutingAssembly().Location + " /e %1");
                        if (newReg != null) { newReg.Close(); newReg = null; }
                    }
                }
                
                if (reg != null) { reg.Close(); reg = null; }
            }
            catch(UnauthorizedAccessException)
            {
                //MessageBox.Show("没有权限修改右键菜单，请以管理员权限打开程序");
            }
            catch(Exception e)
            {
                //MessageBox.Show(e.Message);
            }
            finally
            {
                if(reg != null)
                    reg.Close();
                if (newReg != null)
                    newReg.Close();
            }
        }

        private void SetDecryptRegistryKey()
        {
            RegistryKey reg = null;
            RegistryKey newReg = null;
            try
            {
                reg = Registry.LocalMachine;
                reg = reg.OpenSubKey(@"Software\Classes\*\shell\DecryptEntry");
                //如果注册表不存在该项，则创建
                if (null == reg)
                {
                    newReg = Registry.LocalMachine;
                    newReg = newReg.CreateSubKey(@"Software\Classes\*\shell\DecryptEntry");
                    newReg.SetValue("", "将文件解密");
                    if (newReg != null) { newReg.Close(); newReg = null; }
                }

                //再次检查是否存在EncryptTool项
                reg = Registry.LocalMachine;
                reg = reg.OpenSubKey(@"Software\Classes\*\shell\DecryptEntry", true);
                if (reg != null)
                {
                    //运行至此右键菜单应该已经创建，下面设置图标
                    object temp = reg.GetValue("Icon");
                    if (temp == null)
                    {
                        //设置图标
                        reg.SetValue("Icon", Assembly.GetExecutingAssembly().Location);
                        reg.Close(); reg = null;
                    }

                    //设置右键菜单命令
                    reg = Registry.LocalMachine;
                    reg = reg.OpenSubKey(@"Software\Classes\*\shell\DecryptEntry\command");
                    //如果存在该注册表项则跳过
                    if (null == reg)
                    {
                        newReg = Registry.LocalMachine;
                        newReg = newReg.CreateSubKey(@"Software\Classes\*\shell\DecryptEntry\command");
                        newReg.SetValue("", Assembly.GetExecutingAssembly().Location + " /d %1");
                        if (newReg != null) { newReg.Close(); newReg = null; }
                    }
                }

                if (reg != null) { reg.Close(); reg = null; }
            }
            catch (UnauthorizedAccessException)
            {
                //MessageBox.Show("没有权限修改右键菜单，请以管理员权限打开程序");
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
            finally
            {
                if (reg != null)
                    reg.Close();
                if (newReg != null)
                    newReg.Close();
            }
        }
    }
}
