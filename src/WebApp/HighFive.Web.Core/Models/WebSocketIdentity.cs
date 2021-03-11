using System;
using System.Collections.Generic;
using System.Text;

namespace HighFive.Web.Core.Models
{
    public class WebSocketIdentity
    {
        public string Id { get; set; }
        public string RoomId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public bool IsInRoom { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as WebSocketIdentity).Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
