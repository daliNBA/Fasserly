using System;
using System.Collections.Generic;
using System.Text;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class BuyerDto
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsOwner { get; set; }
        public bool Following { get; set; }
    }
}
