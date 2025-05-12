import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const connection = new HubConnectionBuilder()
   .withUrl("http://localhost:5048/hub/notifications")
   .withAutomaticReconnect()
   .configureLogging(LogLevel.Information)
   .build();

export function startSignalR(onNotification: (title: string, message: string) => void) {
   connection
      .start()
      .then(() => console.log("SignalR connected"))
      .catch(err => console.error("SignalR error:", err));

   connection.on("ReceiveNotification", ({ title, message }) => {
      onNotification(title, message);
   });
}
