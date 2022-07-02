

namespace BlazorDevIta.UI.Tests.Components;

[TestFixture]
public class TextBoxFixture
{
    [Test]
    public void ValueDoesNotNull_ShouldViewValue()
    {
        var ctx = new Bunit.TestContext();

        var id = Guid.NewGuid().ToString();
        var value = "test";
        var label = "label";
        Action<string> valueChanged = (s) => value = s;

        //Viene inizializzato un componente.
        var textBox = ctx.RenderComponent<TextBox>(ps =>
            //Va aggiunto anche l'edit context dato che il componente è figlio di una form che automaticamente passa l'edit context ai suoi figli.    
            ps.AddCascadingValue(new EditContext(value))
                .Add(p => p.Id, id)
                .Add(p => p.Label, label)
                .Add(p => p.Value, value)
                .Add(p => p.ValueChanged, valueChanged)
                .Add(p => p.ValueExpression, () => value)
        );

        textBox.MarkupMatches(@$"<div class=""form-group"">
            <label for={id}>{label}</label >
            <input id={id} class=""form-control valid""
                tvalue=""string""
                value=""{value}""/>
        </div>");
    }

    [Test]
    public void ChangeValue_ShouldViewNewValueAndChangeParameter()
    {
        var ctx = new Bunit.TestContext();

        var id = Guid.NewGuid().ToString();
        var value = "test";
        var label = "label";
        Action<string> valueChanged = (s) => value = s;

        //Viene inizializzato un componente.
        var textBox = ctx.RenderComponent<TextBox>(ps =>
            //Va aggiunto anche l'edit context dato che il componente è figlio di una form che automaticamente passa l'edit context ai suoi figli.    
            ps.AddCascadingValue(new EditContext(value))
                .Add(p => p.Id, id)
                .Add(p => p.Label, label)
                .Add(p => p.Value, value)
                .Add(p => p.ValueChanged, valueChanged)
                .Add(p => p.ValueExpression, () => value)
        );

        //Viene recuperato un input ed assegnato un valore.
        var input = textBox.Find(".form-control");
        var newValue = "new value";
        input.Change(newValue);

        textBox.MarkupMatches(@$"<div class=""form-group"">
            <label for={id}>{label}</label >
            <input id={id} class=""form-control valid modified""
                tvalue=""string""
                value=""{newValue}""/>
        </div>");

        Assert.AreEqual(value, newValue);
    }

    [Test]
    public void ChangeValueWithEmptyValueEditContextInvalid_ShouldViewErrorValidationMessage()
    {
        var ctx = new Bunit.TestContext();

        var id = Guid.NewGuid().ToString();
        var label = "label";
        //Viene definito un model da dare in pasto all'edit context del form.
        var model = new TestModel();
        model.Text = "test";
        var editContext = new EditContext(model);
        Action<string> valueChanged = (s) => model.Text = s;

        var textBox = ctx.RenderComponent<TextBox>(ps =>
            ps.AddCascadingValue(editContext)
                .Add(p => p.Id, id)
                .Add(p => p.Label, label)
                .Add(p => p.Value, model.Text)
                .Add(p => p.ValueChanged, valueChanged)
                .Add(p => p.ValueExpression, () => model.Text)
        );

        //Viene abilitata la validazione con data annotations (nel componente si fa con DataAnnotationValidator)
        editContext.EnableDataAnnotationsValidation();
        var input = textBox.Find(".form-control");
        var newValue = "";
        input.Change(newValue);

        textBox.MarkupMatches(@$"<div class=""form-group"">
          <label for={id}>{label}</label>
          <input id={id} tvalue = ""string"" aria-invalid=""true"" class=""form-control modified invalid"" value={newValue}>
          <div class=""validation-message"">The Text field is required.</div>
        </div>");

        Assert.AreEqual(model.Text, newValue);
    }

    //Classe da passare in input all'edit context.
    private class TestModel
    {
        [Required]
        public string? Text { get; set; }
    }
}
