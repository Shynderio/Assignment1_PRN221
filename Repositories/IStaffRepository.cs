using Estore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Repositories
{
    public interface IStaffRepository
    {
        IEnumerable<Staff> GetStaffList();
        void AddNewStaff(Staff staff);
        void UpdateStaff(Staff staff);
        void DeleteStaff(Staff staff);
        
        Staff GetStaffById(int id);
    }
}
