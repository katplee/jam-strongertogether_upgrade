using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEngine.ResourceManagement
{
    internal class WebRequestQueueOperation
    {
        public UnityWebRequestAsyncOperation Result;
        public Action<UnityWebRequestAsyncOperation> OnComplete;

        public bool IsDone
        {
            get { return Result != null; }
        }

        internal UnityWebRequest m_WebRequest;

        public WebRequestQueueOperation(UnityWebRequest request)
        {
            m_WebRequest = request;
        }

        internal void Complete(UnityWebRequestAsyncOperation asyncOp)
        {
            Result = asyncOp;
            OnComplete?.Invoke(Result);
        }
    }


    internal static class WebRequestQueue
    {
        internal static int s_MaxRequest = 500;
        internal static Queue<WebRequestQueueOperation> s_QueuedOperations = new Queue<WebRequestQueueOperation>();
        internal static List<UnityWebRequestAsyncOperation> s_ActiveRequests = new List<UnityWebRequestAsyncOperation>();
        public static void SetMaxConcurrentRequests(int maxRequests)
        {
            if (maxRequests < 1)
                throw new ArgumentException("MaxRequests must be 1 or greater.", "maxRequests");
            s_MaxRequest = maxRequests;
        }

        public static WebRequestQueueOperation QueueRequest(UnityWebRequest request)
        {
            WebRequestQueueOperation queueOperation = new WebRequestQueueOperation(request);
            if (s_ActiveRequests.Count < s_MaxRequest)
            {
                var webRequestAsyncOp = request.SendWebRequest();
                s_ActiveRequests.Add(webRequestAsyncOp);

                if (webRequestAsyncOp.isDone)
                    OnWebAsyncOpComplete(webRequestAsyncOp);
                else
                    webRequestAsyncOp.completed += OnWebAsyncOpComplete;

                queueOperation.Complete(webRequestAsyncOp);
            }
            else
                s_QueuedOperations.Enqueue(queueOperation);

            return queueOperation;
        }

        private static void OnWebAsyncOpComplete(AsyncOperation operation)
        {
            s_ActiveRequests.Remove((operation as UnityWebRequestAsyncOperation));

            if (s_QueuedOperations.Count > 0)
            {
                var nextQueuedOperation = s_QueuedOperations.Dequeue();
                var webRequestAsyncOp = nextQueuedOperation.m_WebRequest.SendWebRequest();
                webRequestAsyncOp.completed += OnWebAsyncOpComplete;
                s_ActiveRequests.Add(webRequestAsyncOp);
                nextQueuedOperation.Complete(webRequestAsyncOp);
            }
        }
    }
}
