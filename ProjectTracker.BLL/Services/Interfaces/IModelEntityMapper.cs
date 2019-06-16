using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Interfaces
{
    public interface IModelEntityMapper<TModel, TEntity> 
        where TModel : class
        where TEntity : class
    {
        TModel ConvertToModel(TEntity entity);
        TEntity ConvertToEntity(TModel model);
    }
}
