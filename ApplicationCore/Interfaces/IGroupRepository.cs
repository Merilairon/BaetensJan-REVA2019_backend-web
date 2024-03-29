using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetAll();
        Task<List<Group>> GetAllLight();
        Task<Group> GetById(int groupId);
        Task Add(Group group);
        Task<Group> AddMember(int id, string member);
        Task<Group> RemoveMember(int id, string member);
        void Update(Group group);
        void Remove(Group group);
        Task SaveChanges();
    }
}