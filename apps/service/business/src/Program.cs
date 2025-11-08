using Amazon.CDK;
using Imagineerable.Service.Business.src.StackConstructors;
using Imagineerable.Service.Business.Stacks;

App app = new App();

CdkConstructor.FromApp(app);

CdkConstructor.CreateStack<ServiceEmailStack>(app, "ServiceEmailStack");
CdkConstructor.CreateStack<ServiceCognitoStack>(app, "ServiceCognitoStack");

app.Synth();