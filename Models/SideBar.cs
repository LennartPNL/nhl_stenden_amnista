using Amnista.Data_Types.Enums;


namespace Amnista.Models
{
    class SideBar
    {
        public string UserName { get; set; }
        public string ServerAddress { get; set; }
        public Status Status { get; set; }
        public int UsersOnline { get; set; }
    }
}