/*
 * Written by:
 * Henrik
*/

using TMPro;

public class vendingMachineButton_Interactable : Interactable
{
    public TextMeshPro KeyInputs;
    public KeypadInput buttonInput;


    public override void InteractTrigger(object target = null)
    {
        switch(buttonInput)
        {
            case KeypadInput.One:
                if(KeyInputs.text.Length < 2)
                    KeyInputs.text += 1;
                break;
            case KeypadInput.Two:
                if(KeyInputs.text.Length < 2)
                    KeyInputs.text += 2;
                break;
            case KeypadInput.Three:
                if(KeyInputs.text.Length < 2)
                    KeyInputs.text += 3;
                break;
            case KeypadInput.Four:
                if(KeyInputs.text.Length < 2)
                    KeyInputs.text += 4;
                break;
            case KeypadInput.Five:
                if(KeyInputs.text.Length < 2)
                    KeyInputs.text += 5;
                break;
            case KeypadInput.Six:
                if(KeyInputs.text.Length < 2)
                    KeyInputs.text += 6;
                break;
            case KeypadInput.Seven:
                if(KeyInputs.text.Length < 2)
                    KeyInputs.text += 7;
                break;
            case KeypadInput.Eight:
                if(KeyInputs.text.Length < 2)
                    KeyInputs.text += 8;
                break;
            case KeypadInput.Nine:
                if(KeyInputs.text.Length < 2)
                    KeyInputs.text += 9;
                break;
            case KeypadInput.Zero:
                if(KeyInputs.text.Length < 2)
                    KeyInputs.text += 0;
                break;

            case KeypadInput.Enter:
                GetComponentInParent<VendingMachineController>().buyTower();
                break;

            case KeypadInput.Back:
                char[] c = KeyInputs.text.ToCharArray();
                char[] cNew = new char[c.Length-1];
                for (int i = 0; i < cNew.Length; i++)
                {
                    cNew[i] = c[i];
                }
                KeyInputs.text = cNew.ArrayToString();
                break;
        }
    }

    public override void InteractionEndTrigger(object target = null)
    {

    }
}

public enum KeypadInput
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Zero,
    Back,
    Enter
}