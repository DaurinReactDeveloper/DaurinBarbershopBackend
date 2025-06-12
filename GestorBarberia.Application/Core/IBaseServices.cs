using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Core
{
    public interface IBaseServices<AddDto,RemoveDto,UpdateDto>
    {
        ServiceResult Add(AddDto modelDto);
        ServiceResult Remove(RemoveDto modelDto);
        ServiceResult Update(UpdateDto modelDto);
    }
}
