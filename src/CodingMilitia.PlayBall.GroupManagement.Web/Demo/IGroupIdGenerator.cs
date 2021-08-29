using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Demo
{
    public interface IGroupIdGenerator
    {
        long Next();
    }
}
