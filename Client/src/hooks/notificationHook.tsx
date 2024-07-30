import { useState } from "react";
import Notification from "../components/shared/Notification";

const useNotification = () => {
  const [message, setMessage] = useState<string>("");
  const [show, setShow] = useState<boolean>(false);

  const showNotification = (message: string) => {
    setMessage(message);
    setShow(true);
  };

  const hideNotification = () => {
    setShow(false);
  };

  const NotificationComponent = () => {
    return ( <>
      {show && <Notification message={message} onClose={hideNotification}/>}
    </>
    );
  };

  return { showNotification, hideNotification, NotificationComponent };
};

export default useNotification;