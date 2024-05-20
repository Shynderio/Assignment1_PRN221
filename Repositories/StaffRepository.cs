using Estore.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Repositories
{
    class StaffRepository : IStaffRepository
    {
        private readonly MyStoreContext _storeContext;

        public StaffRepository(MyStoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public void AddNewStaff(Staff staff)
        {
            _storeContext.Staffs.Add(staff);
            _storeContext.SaveChanges();
        }

        public void DeleteStaff(Staff staff)
        {
            try
            {
                var staffCheck = _storeContext.Staffs.SingleOrDefault(p => p.StaffId == staff.StaffId);
                if (staffCheck != null)
                {
                    _storeContext.Staffs.Remove(staff);
                    _storeContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The staff isn't exits.");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }


        }

        public Staff GetStaffById(int id)
        {
            
            try
            {
                var staffCheck = _storeContext.Staffs.SingleOrDefault(p => p.StaffId == id);
                if (staffCheck != null)
                {
                   return staffCheck;
                }
                else
                {
                    throw new Exception("The staff isn't exits.");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
        }

        public IEnumerable<Staff> GetStaffList()
        {
            List<Staff> list;
            list = _storeContext.Staffs.ToList();
            return list;
        }

        public void UpdateStaff(Staff staff)
        {
            try
            {
                var staffCheck = _storeContext.Staffs.SingleOrDefault(p => p.StaffId == staff.StaffId);
                if (staffCheck != null)
                {
                    _storeContext.Entry<Staff>(staff).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _storeContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The staff isn't exits.");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
        }
    }
}
