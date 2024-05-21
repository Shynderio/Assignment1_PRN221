using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Estore.Session_Login
{
    internal class SessionManage
    {
        private static SessionManage instance;

        private SessionManage()
        {
        }

        public static SessionManage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SessionManage();
                }
                return instance;
            }
        }

        public void SetSession(string key, object value)
         //cho phép lưu 1 giá trị vào session với một key nhất định.
         //Nó sử dụng Application.Current.Properties để lưu trữ dữ liệu session.
        {
            Application.Current.Properties[key] = value;
        }

        public object GetSession(string key)
        {
            return Application.Current.Properties[key];
        }

        public void RemoveSession(string key)
        {
            Application.Current.Properties.Remove(key);
        }
    }
}