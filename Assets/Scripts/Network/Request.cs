using System;
namespace Com.Nextplease.IWT
{
    [Serializable]
    public class Request
    {
        private string requester;
        private readonly byte actionType;
        private bool approved;
        private readonly Data data;

        public Request(byte actionType, Data data)
        {
            this.actionType = actionType;
            this.approved = false;
            this.data = data;
        }

        public void Approve()
        {
            this.approved = true;
        }

        public bool IsApproved()
        {
            return this.approved;
        }

        public string GetRequester()
        {
            return this.requester;
        }

        public void SetRequester(string requester)
        {
            this.requester = requester;
        }

        public byte GetActionType()
        {
            return this.actionType;
        }

        public Data GetData()
        {
            return this.data;
        }

    }
}
