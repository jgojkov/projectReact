import { Route } from "react-router-dom";


const PublicRoute = ({
  layout: LayoutWrapper,
  component: Component,
  ...rest
}) => {
  return (
    <Route
      {...rest}
      render={(props) => (
        <LayoutWrapper>
          <Component {...props} />
        </LayoutWrapper>
      )}
    />
  );
};

export default PublicRoute;