using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;  // 移动速度
    public float rotationSpeed = 700f;  // 旋转速度

    private void Update()
    {
        // 获取输入
        float horizontal = Input.GetAxis("Horizontal"); // A/D 或 左/右箭头
        float vertical = Input.GetAxis("Vertical"); // W/S 或 上/下箭头

        // 计算移动方向
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 移动角色
        if (moveDirection.magnitude >= 0.1f)
        {
            // 计算目标旋转
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // 移动角色
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
