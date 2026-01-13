import { useEffect, useState } from "react";

export const HomePage = () => {

  const [products, setProducts] = useState([]);

  useEffect(() => {
    const fechProducts = async () => {
      try {
        const response = await fetch("https://localhost:7189/products");
        const data = await response.json();
        setProducts(data);
      } catch (error) {
        console.error("Error fetching products:", error);
      }
    };

    fechProducts();
  }, []);

  return (
    <div className="row">
      <div className="col">
        <h2>Welcome</h2>
      </div>
    </div>
  );
}
