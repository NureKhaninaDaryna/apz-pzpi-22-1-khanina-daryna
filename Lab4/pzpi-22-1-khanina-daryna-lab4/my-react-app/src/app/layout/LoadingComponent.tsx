import { Loader } from "semantic-ui-react";

interface Props {
   content?: string;
}

export default function LoadingComponent({ content = "Loading..." }: Props) {
   return (
      <div style={{
         display: "flex",
         justifyContent: "center",
         alignItems: "center",
         height: "100vh",
         width: "100vw"
      }}>
         <Loader active inline="centered" content={content} />
      </div>
   );
}
