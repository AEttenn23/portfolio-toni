// TaskBase.cs (extracto) - clase base abstracta: cada minijuego solo implementa IniciarTarea y CancelarTarea
public abstract class TaskBase : MonoBehaviour
{
    #region Public Interaction

    public virtual void Interactuar()
    {
        if (!EstaEnListaDeTareas()) return;

        if (!feedbackcanvas.brazoYaCaido)
        {
            if (playerCerca && !tareaAcabada && !interactuando)
            {
                uiTarea.SetActive(true);

                interactuando = true;

                if (player != null)
                    player.GetComponent<PlayerController>().playerOcupado = true;

                canvasInteractKey?.SetActive(false);

                IniciarTarea();
            }

        }
    }

    #endregion

    #region Base States

    protected void CompletarTarea()
    {
        if (taskExclamation != null)
            taskExclamation.SetActive(false);
        taskManager.CompletarTarea(this.gameObject);
        tareaAcabada = true;
        interactuando = false;

        if (particles != null) particles.SetActive(false);
        if (uiTarea != null) uiTarea.SetActive(false);
        if (canvasInteractKey != null) canvasInteractKey.SetActive(false);
        if (player != null) player.GetComponent<PlayerController>().playerOcupado = false;

        StopAllCoroutines();
        this.enabled = false;
    }

    protected void CancelarBase()
    {
        interactuando = false;

        if (uiTarea != null) uiTarea.SetActive(false);
    // ...
    #region Abstract

    protected abstract void IniciarTarea();
    protected abstract void CancelarTarea();

    #endregion
}
