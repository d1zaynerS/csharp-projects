using StoreAndBookingApp.Models;

namespace StoreAndBookingApp.Services
{
    public interface IBookingService
    {
        List<Room> GetRooms();
        Room? GetRoom(int id);
        List<Booking> GetBookings();
        bool IsAvailable(int roomId, DateTime start, DateTime end);
        bool AddBooking(Booking booking);
        void CancelBooking(int id);
    }
}