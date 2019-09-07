namespace Com.Nextplease.IWT
{
    public class Request
    {
        private readonly string requester;
        private readonly byte actionType;
        private bool approved;
        private readonly Data data;


        public Request(string requester, byte actionType, Data data)
        {
            this.requester = requester;
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
