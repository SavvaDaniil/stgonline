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
        public IEnumerable<Style> listAll()
        {
            return this._dbc.Styles.OrderByDescending(p => p.id).ToList();
        }

        public List<Style> listAllActive()
        {
            return this._dbc.Styles.Where(p => p.active == 1).OrderByDescending(p => p.id).ToList();
        }

        public Style findById(int id)
        {
            return this._dbc.Styles.FirstOrDefault(p => p.id == id);
        }

        public Style add(StyleNewDTO styleNewDTO)
        {
            Style style = new Style();
            style.name = styleNewDTO.name;

            this._dbc.Styles.Add(style);

            this._dbc.SaveChanges();

            return style;
        }

        public bool update(StyleDTO styleDTO)
        {
            Style style = findById(styleDTO.id);

            if (style == null) return false;

            style.name = styleDTO.name;
            style.active = styleDTO.active;

            this._dbc.SaveChanges();

            return true;
        }

        public bool delete(int id)
        {
            Style style = findById(id);
            this._dbc.Styles.Remove(style);
            this._dbc.SaveChanges();
            return true;
        }
    }
}
