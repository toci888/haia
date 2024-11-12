export const endpoints = {
  getPosts: (userId: number) => `PostInteraction/suggested/${userId}`,
  addCommentToPost: `Comment`,
  getCommentsByPostId: (postId: number) => `Comment/postComments/${postId}`,
}









