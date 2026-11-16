using StoreAndBookingApp.Models;

namespace StoreAndBookingApp.Services
{
    public class BookingService : IBookingService
    {
        private readonly List<Room> _rooms = new()
        {
            new Room { Id = 1, Name = "Meeting Room Alpha" },
            new Room { Id = 2, Name = "Conference Hall Beta" }
        };
        private readonly List<Booking> _bookings = new();
        private int _nextId = 1;

        public List<Room> GetRooms() => _rooms;
        public Room? GetRoom(int id) => _rooms.FirstOrDefault(r => r.Id == id);
        public List<Booking> GetBookings() => _bookings;

        public bool IsAvailable(int roomId, DateTime start, DateTime end)
        {
            if (start < DateTime.Now || end <= start) return false;
            return !_bookings.Any(b => b.RoomId == roomId && b.StartTime < end && b.EndTime > start);
        }

        public bool AddBooking(Booking booking)
        {
            if (!IsAvailable(booking.RoomId, booking.StartTime, booking.EndTime)) return false;
            booking.Id = _nextId++;
            _bookings.Add(booking);
            return true;
        }

        public void CancelBooking(int id) => _bookings.RemoveAll(b => b.Id == id);
    }
}
