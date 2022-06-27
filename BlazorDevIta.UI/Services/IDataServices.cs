using BlazorDevIta.ERP.Infrastructure.DataTypes;

namespace BlazorDevIta.UI.Services;

public interface IDataServices<ListItemType, DetailsType, IdType>
    where ListItemType : BaseListItem<IdType>
{
    Task<Page<ListItemType, IdType>> GetAllAsync(PageParameters pageParameters);

    Task<DetailsType?> GetByIdAsync(IdType id);

    Task CreateAsync(DetailsType details);

    Task UpdateAsync(DetailsType details);

    Task DeleteAsync(IdType id);
}
