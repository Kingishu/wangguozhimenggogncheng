using System;
using System.Collections;
using UnityEngine;

public class CameraDragController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float minX;    // 左边界
    public float maxX;     // 右边界
    public float sensitivity = 0.1f; // 拖拽灵敏度

    private Vector3 dragOrigin; // 鼠标拖拽起点（屏幕坐标）
    private Vector3 cameraOrigin; // 摄像机初始位置
    private bool isDragging = false;
    
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleDragInput();
    }

    void HandleDragInput()
    {
        // 鼠标按下时记录初始位置
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            cameraOrigin = transform.position;
            isDragging = true;
        }

        // 鼠标松开时结束拖拽
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // 拖拽过程中更新位置
        if (isDragging)
        {
            // 计算鼠标位移（转换为视口坐标，适配不同分辨率）
            Vector3 delta = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            // 计算新的水平位置（拖拽方向与移动方向相反）
            float newX = cameraOrigin.x - delta.x * sensitivity * 100;
            
            // 限制移动范围
            newX = Mathf.Clamp(newX, minX, maxX);
            
            // 更新摄像机位置（保持Y轴和Z轴不变）
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    // 可视化调试边界（在Scene视图中显示）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minX, transform.position.y - 10, 0), 
            new Vector3(minX, transform.position.y + 10, 0));
        Gizmos.DrawLine(new Vector3(maxX, transform.position.y - 10, 0), 
            new Vector3(maxX, transform.position.y + 10, 0));
    }
    //加载房间的时候关闭这个组件
    public void LoadRoomEvent()
    {
        Invoke("MoveCameraToMid",0.5f);
        this.enabled = false;
    }
    //加载地图的时候打开组件
    public void BackToMap()
    {
        Invoke("MoveCameraToLeft",0.5f);
        this.enabled = true;
    }

    public void MoveCameraToLeft()
    {
        transform.position = new Vector3(-12f, 0, -10);
    }
    public void MoveCameraToMid()
    {
        transform.position = new Vector3(0, 0, -12);
    }

    public void PlayClipMoveCamera()
    {
        animator.SetTrigger("Play");
        StartCoroutine(DelayDeleteAnimator());

    }

    IEnumerator DelayDeleteAnimator()
    {
        yield return new WaitForSeconds(5.2f);
      //  transform.position= new Vector3(-10.36f, 0, -10);
        animator.enabled = false;
    }
}