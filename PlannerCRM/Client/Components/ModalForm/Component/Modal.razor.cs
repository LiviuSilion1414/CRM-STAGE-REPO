using System.ComponentModel;

namespace PlannerCRM.Client.Components.ModalForm.Component;

public partial class Modal : ComponentBase
{
    [Parameter] public string Size { get; set; } = ModalSize.DEFAULT;
    [Parameter] public string Title { get; set; } = Titles.DEFAULT_MODAL_TITLE;
    [Parameter] public Dictionary<string, object> AdditionalAttributes { get; set; }

    [Parameter] public RenderFragment Header { get; set; }
    [Parameter] public RenderFragment Body { get; set; }
    [Parameter] public RenderFragment Footer { get; set; }

    private bool _isCancelClicked = false;
    private bool _isShowWarningModalClicked = false;

    private void ShowWarningModal()
        => _isShowWarningModalClicked = !_isShowWarningModalClicked;

    private void CloseModal()
    {
        _isCancelClicked = !_isCancelClicked;
    }

    private void HandleConfirmedAction(bool isConfirmed)
    {
        if (isConfirmed)
        {
            CloseModal();
        }
    }
}