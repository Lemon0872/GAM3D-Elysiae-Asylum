using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    /*Hướng dẫn sử dụng:
        Với bất kỳ script nào dùng để tương tác, vd: Destroy : MonoBehaviour
        Thì thêm IInteractable sau MonoBehavior rồi quickfix Implement interface
        VD: Destroy : MonoBehaviour, IInteractable
        Sau đó fix lại code như bên dưới
    */
    void Interact(Transform interactorTransform); /*Thêm cái method chức năng tương tác cần vào
    {
        METHOD();
    }
    public void METHOD()
    {
        Destroy(gameObject);
    }*/


    string GetInteractText(); /*thêm return interactText như comment dưới
    {
        return interactText;
    }*/
    Transform GetTransform(); /*thêm return transform như comment dưới
        {
        return transform;
    }*/

}