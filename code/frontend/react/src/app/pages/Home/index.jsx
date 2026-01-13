import { useEffect, useRef, useState } from "react";

export const HomePage = () => {

  const [createdOrder, setCreatedOrder] = useState(false);
  const [products, setProducts] = useState([]);
  
  const [searchedProducts, setSearchedProducts] = useState([]);
  const [savedProducts, setSavedProducts] = useState([]);

  const searchInput = useRef(null);
  const customerRef = useRef(null);
  
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

  const onSearchProducts = () => {
    const searchedTerm = searchInput.current.value;
    if(searchedTerm.trim() === "") return;

    const searchedId = Number(searchedTerm);

    //TODO: skipt letters
    const searchedResults = products.filter(product => product.id == searchedId);
    
    setSearchedProducts(searchedResults);
  };

  const onSavedProduct = (product) => {
    // TODO: delete validation, increase quantity
    const isAlreadySaved = savedProducts.some(savedProduct => savedProduct.id === product.id);
    if (isAlreadySaved) return;

    setSavedProducts([...savedProducts, product]);
  }

  const onHandleDelete = (productId) => {
    const updatedProducts = savedProducts.filter(product => product.id !== productId);
    setSavedProducts(updatedProducts);
  }

  const submitOrder = async () => {
     

     try {
      const customerId = customerRef.current.value;

      const orderData = {
        customerId: Number(customerId),
        items: savedProducts.map(product => ({
          Id: product.id,
          quantity: 1 // TODO: handle quantity
        }))
      };
      debugger;
     const response = await fetch("https://localhost:7189/orders", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(orderData)
      });

      if(response.status !== 200) {
        throw new Error("Failed to submit order");
      }

      const savedOrder = await response.json();

      alert("Order submitted successfully!", savedOrder.orderId);
     }
     catch (error) {
      alert("Error submitting order:", error);
     }
  }

  if (products.length === 0) {
    return <div>Loading...</div>;
  }

  return (
    <div className="container row">
      <div className="col">
        <h2>Orders</h2>
        <div className="row">
            {/* {products.map((product) => (
              <div key={product.id}>
                {product.name} - ${product.price}
              </div>
            ))} */}

            <button className="btn btn-primary col" disabled={createdOrder} onClick={() => setCreatedOrder(true)}>Create New Order</button>
            <button className="btn btn-secondary col" disabled={!createdOrder} onClick={() => setCreatedOrder(false)}>Clean Order</button>
        </div>
        
        <div>
          {createdOrder ? (
            <>
            <div>
              <h3>New Order Form</h3>
              <div className="mb-3">
                <label htmlFor="customerId" className="form-label">Customer ID</label>
                <input ref={customerRef} type="text" className="form-control" id="customerId" placeholder="Enter Customer Id" />
              </div>
              <div className="mb-3">
                <label htmlFor="productSearch" className="form-label">Search Products</label>
                <input ref={searchInput} type="text" className="form-control" id="productSearch" placeholder="Search products by Ids" />
                <button className="btn btn-primary" onClick={onSearchProducts}>Search</button>
              </div>

              <div>
              <h3>Search Results</h3>
              <table className="table">
                <thead>
                  <tr>
                    <th scope="col">Product ID</th>
                    <th scope="col">Name</th>
                    <th scope="col">Price</th>
                  </tr>
                </thead>
                <tbody>
                  {searchedProducts.map((product) => (
                    <tr key={product.id}>
                      <td>{product.id}</td>
                      <td>{product.name}</td>
                      <td>${product.price}</td>
                      <td><button onClick={() => onSavedProduct(product)}>Add to Order</button></td>
                    </tr>
                  ))}
                </tbody>
              </table>
              
            </div>

            </div>

            { savedProducts.length > 0 && (
              <>
              <div className="mb-4">
                <h2>Summary</h2>
                <h3>Products in Order</h3>
                <div>
                  {savedProducts.map((product) => (
                    <div key={product.id} className="row">
                      <div className="col">
                      {product.name} - ${product.price}
                      </div>
                      <div className="col">
                        <button onClick={() => onHandleDelete(product.id)}>Delete</button>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
              <div>
                <button className="btn btn-success" onClick={submitOrder}>Submit Order</button>
              </div>
              </>
            )}
            
            </>

          ) : (
            <div>
              <h3>No active order</h3>
            </div>
          )}
        </div>

      </div>
    </div>
  );
}
