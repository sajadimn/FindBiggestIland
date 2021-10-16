using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceOfWorldUi : MonoBehaviour
{
    public Text pieceValue = null;
    public Image pieceBack = null;
    public int row;
    public int column;
    public int value;

    public void Init(int row , int column , int value)
    {
        this.row = row;
        this.column = column;
        this.value = value;
    }
}
