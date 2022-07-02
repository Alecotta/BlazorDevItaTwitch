namespace BlazorDevIta.UI.Tests.Components;

[TestFixture]
public class DetailsFixture
{
    [Test]
    public void FirstView_ShouldRenderComponentCorrectly()
    {
        var ctx = new Bunit.TestContext();

        var model = new DetailsClassTest();
        model.Text = "test";

        //Questo componente richiede l'utilizzo dei generics.
        var detail = ctx.RenderComponent<Details<DetailsClassTest>>(ps =>
            ps.Add(p => p.Item, model)
            .Add(x => x.Fields, i => $"<span>{i.Text}</span>")
            .Add(x => x.HeaderPropertyName, nameof(model.Text))
        );

        detail.MarkupMatches(@$"
            <h3>Details {model.Text}</h3>
            <form>
              <span>{model.Text}</span>
              <button type=""button"" class=""btn btn-default"">Cancel</button>
              <button type=""submit"" class=""btn btn-primary"">Save</button>
            </form>
        ");
    }

    [Test]
    public void ClickOnSave_ShouldRaiseOnSave()
    {
        var ctx = new Bunit.TestContext();

        var model = new DetailsClassTest();
        model.Text = "test";

        DetailsClassTest onSaveCallbackModel = null;

        var detail = ctx.RenderComponent<Details<DetailsClassTest>>(ps =>
            ps.Add(p => p.Item, model)
            .Add(x => x.Fields, i => $"<span>{i.Text}</span>")
            .Add(x => x.HeaderPropertyName, nameof(model.Text))
            .Add(x => x.OnSave, (item) => onSaveCallbackModel = item)
        );

        var onSaveButton = detail.Find(".btn-primary");
        onSaveButton.Click();

        //In questo modo viene verificato che cliccando sul bottone Save venga invocato il metodo OnSave.
        Assert.AreEqual(onSaveCallbackModel.Text, model.Text);
    }

    private class DetailsClassTest
    {
        [Required]
        public string? Text { get; set; }
    }
}
