using System;
using System.Collections.Generic;
using System.Text;

namespace CodingMilitia.PlayBall.GroupManagement.Data.Entities
{
    public class GroupEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        // uint postgres specific. SQL server would be byte[]
        public uint RowVersion { get; set; }
    }
}
