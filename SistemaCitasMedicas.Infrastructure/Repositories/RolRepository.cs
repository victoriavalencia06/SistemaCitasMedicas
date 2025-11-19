using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Infrastructure.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly AppDBContext _context;

        public RolRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Rol> GetRolByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Rol> AddRolAsync(Rol rol)
        {
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<Rol> UpdateRolAsync(Rol rol)
        {
            _context.Entry(rol).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<bool> DeleteRolAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return false;

            rol.Estado = 0;
            _context.Roles.Update(rol);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateRolAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return false;

            rol.Estado = 1;
            _context.Roles.Update(rol);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            return await _context.Set<Menu>().OrderBy(m => m.Orden).ToListAsync();
        }

        public async Task<IEnumerable<RolMenu>> GetRolMenusByRolIdAsync(int idRol)
        {
            return await _context.Set<RolMenu>().Where(rm => rm.IdRol == idRol).ToListAsync();
        }

        public async Task AddRolMenusAsync(IEnumerable<RolMenu> rolMenus)
        {
            _context.Set<RolMenu>().AddRange(rolMenus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRolMenusAsync(IEnumerable<RolMenu> rolMenus)
        {
            _context.Set<RolMenu>().UpdateRange(rolMenus);
            await _context.SaveChangesAsync();
        }

        public async Task<RolMenu?> GetRolMenuAsync(int idRol, int idMenu)
        {
            return await _context.Set<RolMenu>().FirstOrDefaultAsync(rm => rm.IdRol == idRol && rm.IdMenu == idMenu);
        }

    }
}
