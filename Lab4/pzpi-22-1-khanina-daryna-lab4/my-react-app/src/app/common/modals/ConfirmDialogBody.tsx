import { Button } from "semantic-ui-react";

interface ConfirmDialogBodyProps {
   onConfirm: () => void;
   onCancel: () => void;
   title?: string;
   content?: string;
}

export default function ConfirmDialogBody({
                                             onConfirm,
                                             onCancel,
                                             title = "Are you sure?",
                                             content = "This action cannot be undone."
                                          }: ConfirmDialogBodyProps) {
   return (
      <div>
         <h3>{title}</h3>
         <p>{content}</p>
         <div style={{ display: "flex", justifyContent: "flex-end", gap: "10px", marginTop: "15px" }}>
            <Button onClick={onCancel} color="grey">
               Cancel
            </Button>
            <Button onClick={onConfirm} color="red">
               Yes, Delete
            </Button>
         </div>
      </div>
   );
}