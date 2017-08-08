using System;
using PosWeb.Models.Infratructure;
using Microsoft.EntityFrameworkCore;

namespace PosWeb.Models
{
    [EntityMapping]
    public class TestModel : TrackedEntity
    {
		public static void CreateEntityMapping(ModelBuilder modelBuilder)
		{

		}
    }
}
