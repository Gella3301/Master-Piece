using Kursovaya.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kursovaya
{
    public class DataFacade
    {
        private readonly AppDbContext _context;


        public DataFacade()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();

        }

        public void AddPiece(Piece piece)
        {
            _context.Pieces.Add(piece);
            _context.SaveChanges();

        }
        public void AddSession(Session session)
        { 
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }
        public List<Session> GetAllSessionsWithPieces()
        {
            return _context.Sessions
                .Include(s => s.Piece) // Подгружаем связанное произведение
                .OrderByDescending(s => s.StartTime) // Сортируем: новые сверху
                .ToList();
        }
        public List<Piece> GetAllPieces()
        {
            return _context.Pieces.ToList();
        }
        public List<Session> GetAllSessions()
        {
            return _context.Sessions.ToList();
        }
    }
}
