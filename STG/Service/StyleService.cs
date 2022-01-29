using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO;
using STG.Interface;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.DTO.Style;

namespace STG.Service
{
    public class StyleService
    {
        private ApplicationDbContext _dbc;

        public StyleService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }
        public async Task<IEnumerable<Style>> listAll()
        {
            return await this._dbc.Styles.OrderByDescending(p => p.id).ToListAsync();
        }

        public async Task<List<Style>> listAllActive()
        {
            return await this._dbc.Styles.Where(p => p.active == 1).OrderByDescending(p => p.id).ToListAsync();
        }

        public async Task<Style> findById(int id)
        {
            return await this._dbc.Styles.FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<Style> add(StyleNewDTO styleNewDTO)
        {
            Style style = new Style();
            style.name = styleNewDTO.name;

            this._dbc.Styles.Add(style);

            await this._dbc.SaveChangesAsync();

            return style;
        }

        public async Task<bool> update(StyleDTO styleDTO)
        {
            Style style = await findById(styleDTO.id);

            if (style == null) return false;

            style.name = styleDTO.name;
            style.active = styleDTO.active;

            await this._dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> delete(int id)
        {
            Style style = await findById(id);
            this._dbc.Styles.Remove(style);
            await this._dbc.SaveChangesAsync();
            return true;
        }
    }
}
