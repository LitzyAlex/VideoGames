using System.Collections.Generic;

public enum TipoMision
{
    Imprimir,
    Reporte,
    Excel,
    Llamada,
    Papelera,
    LlevarDocumentos
}

[System.Serializable]
public class Mision
{
    public string nombre;
    public int cantidad = 1;
    public bool puedeRepetirse;

    public List<TaskStation> estaciones;
    public TipoMision tipo;
}