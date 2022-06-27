using BlazorDevIta.ERP.Infrastructure.DataTypes;
using BlazorDevIta.UI.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorDevIta.UI.Pages;

//La classe eredita da ComponentBase così da poterla utilizzare in un componente.
//In questo modo si possono sfruttare gli hook (cicli di vita).
public class BaseCRUDPage<ListItemType, DetailsType, IdType>
    : ComponentBase
    where ListItemType : BaseListItem<IdType>
    where DetailsType : BaseDetails<IdType>, new()
{
    protected Page<ListItemType, IdType>? page;
    protected DetailsType? currentItem = null;

    //Injection by properties.
    [Inject]
    protected IDataServices<ListItemType, DetailsType, IdType>? DataServices { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //A questo punto le properties sono già inizializzate.
        if (DataServices == null)
        {
            throw new Exception("DataServices not provided");
        }

        await RefreshData(new PageParameters());
    }

    protected async Task RefreshData(PageParameters pageParameters)
        => page = await DataServices!.GetAllAsync(pageParameters);

    protected async Task Edit(ListItemType item)
    {
        if (item.Id == null)
        {
            throw new ArgumentNullException("item id cannot be null", "item.Id");
        }

        currentItem = await DataServices!.GetByIdAsync(item.Id);
    }

    protected async Task Delete(ListItemType item)
    {
        if (item.Id == null)
        {
            throw new ArgumentNullException("item id cannot be null", "item.Id");
        }

        await DataServices!.DeleteAsync(item.Id);
        await RefreshData(new PageParameters());
    }

    protected void Cancel()
    {
        currentItem = null;
    }

    protected async Task Save(DetailsType item)
    {
        //if(item.Id != null && item.Id.Equals(default(IdType)))
        //Permette di confrontare un tipo generico con il suo valore di default.
        if (EqualityComparer<IdType>.Default.Equals(item.Id, default(IdType)))
        {
            await DataServices!.CreateAsync(item);
        }
        else
        {
            await DataServices!.UpdateAsync(item);
        }
        await RefreshData(new PageParameters());
        currentItem = null;
    }

    protected void Create()
    {
        currentItem = new();
    }
}
