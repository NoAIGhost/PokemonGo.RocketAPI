using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Login
{
    public interface ILoginProvider
    {
        Task<TokenHolder> Authorize();
    }
}
