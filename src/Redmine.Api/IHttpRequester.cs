namespace Redmine.Api
{
    internal interface IHttpRequester
    {
        IHttpResponse Get(IHttpRequest request);

        IHttpResponse Create(IHttpRequest request);

        IHttpResponse Patch(IHttpRequest request);

        IHttpResponse Update(IHttpRequest request);
    }
}